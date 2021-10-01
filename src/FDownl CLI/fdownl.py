import sys
import os
import argparse
import requests
from bs4 import BeautifulSoup

def main():
    example_usage = '''\
example usage:
  fdownl -d https://fdownl.ga/d23j7
  fdownl -u /home/file.txt
  fdownl -u file1.txt,file2.txt\
'''
    parser = argparse.ArgumentParser(description='Official FDownl CLI', epilog=example_usage, formatter_class=argparse.RawDescriptionHelpFormatter)

    parser.add_argument("-d", help = "download files given a fdownl file page url", dest= "url")
    parser.add_argument("--dest", help = "download the file to this directory - defaults to current directory", dest = "destination", default = ".")
    parser.add_argument("-u", help = "upload files given one or more paths", dest = "path")
    parser.add_argument("--lifetime", help = "upload files with a certain lifetime (in days) - defaults to 7 days",type = int, dest = "lifetime", default = 7)
    parser.add_argument("--coupon", help = "upload files with a certain coupon code", dest = "code", default = "")

    if len(sys.argv) == 1:
        parser.print_help()
        sys.exit()

    args = parser.parse_args()

    if args.url and args.path:
        parser.error("options -d and -u are incompatible")
    elif args.path:
        paths = args.path.split(",")
        files = [('files', open(p, 'rb')) for p in paths]
        print("[DEBUG] Uploading file(s) with 7 days of lifetime")
        id = upload(files, args.lifetime, args.code)
        print(f"[SUCCESS] You can access your files at https://fdow.nl/{id}")
    elif args.url:
        print(f"[DEBUG] Downloading {link}")
        download(args.url, args.destination)
        print(f"[SUCCESS] {fname} saved.")

def upload(files, lifetime = 604800, code = ""):
    return requests.post("https://s1.fdow.nl/upload/", data = {"lifetime": lifetime, "code": code}, files = files).json()["id"]

def download(url, destination):
    resp = requests.get(url)
    soup = BeautifulSoup(resp.text,'html.parser')
    d_btn = soup.find(id = 'download-button')
    link = d_btn['href']
    fname = link.split("/")[-1]
    fname = "-".join(fname.split("-")[1:])
    with open(os.path.join(destination, fname), "wb") as f:
        r = requests.get(link)
        r = r.content
        f.write(r)

if __name__ == "__main__":
    main()
