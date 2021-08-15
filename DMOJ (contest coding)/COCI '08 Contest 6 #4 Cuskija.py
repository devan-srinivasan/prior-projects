"""Cuskija"""
import sys
n = input()
linein = list(map(int, input().split()))
a = []
b = []
c = []
for r in linein:
    if r % 3 == 0:
        a.append(r)
    elif r % 3 == 1:
        b.append(r)
    else:
        c.append(r)
delim = 69
if len(a) > len(b) + len(c) + 1 or (len(a) == 0 and b != [] and c != []):
    print("impossible")
    sys.exit()
out = []
if len(a) != 0:
    delim = a.pop()
d = min(len(a), len(b))
for i in range(0, d):
    out.append(a[i])
    out.append(b[i])
a = a[d::]
b = b[d::]
for B in b:
    out.append(B)
if delim != 69:
    out.append(delim)
d = min(len(a), len(c))
for i in range(0, d):
    out.append(c[i])
    out.append(a[i])
c = c[d::]
for C in c:
    out.append(C)
print(" ".join(str(e) for e in out))
