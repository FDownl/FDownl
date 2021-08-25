import sys
import requests

if len(sys.argv) == 1:
    print(f"[USAGE] python3 {sys.argv[0]} <path_to_file>")
    print(f"[USAGE] python3 {sys.argv[0]} <path_to_file1,path_to_file2,path_to_file3...>")
    exit(0)

path = sys.argv[1]
paths = path.split(",")

files = [('files', open(p, 'rb')) for p in paths]

print("[DEBUG] Uploading file(s)")

r = requests.post("https://s1.fdownl.ga/upload/", data = {"lifetime": 604800, "code": "90DAYSOFLIFETIMEFORCTFSERVER"}, files = files)
r = r.json()

print(f"[RESULT] https://fdownl.ga/{r['id']}")