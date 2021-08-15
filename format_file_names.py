"""format all files to remove timestamp from their names

story: when backing up my laptop I accidentally had timestamps enabled
so every single file (including the thousands nested in my Unity projects)
were renamed with this additional timestamp, but they were not refactored
meaning that any usage of them would break the game. So, being a programmer,
I made this to remove all timestamps recursively :)
"""
import os
PATH = r"C:\Users\mrmac\Desktop"

def ren(filename: str) -> str:
    """Renames files"""
    if '(2021_' not in filename or 'UTC' not in filename:
        return filename
    start, end = None, None
    for i in range(len(filename)):
        if filename[i:i+7] == ' (2021_' and start is None:
            start = i
        elif filename[i:i + 6] == '(2021_' and start is None:
            start = i
        if filename[i:i+4] == 'UTC)' and end is None:
            end = i+4
            break
    return filename[0:start] + filename[end::]


def rec(start: str) -> None:
    """recursively renames each file"""
    if os.path.isdir(PATH + "\\" + start):
        os.chdir(PATH + "\\" + start)
        for dir in os.listdir():
            rec(start + "\\" + dir)
    pathList = start.split("\\")
    fileName = pathList[-1]
    os.chdir(PATH + "\\" + "\\".join(pathList[:-1]))
    #print(' full: ' + PATH + '\\' + start)
    #print(fileName, ren(fileName))
    try:
        os.rename(fileName, ren(fileName))
    except PermissionError:
        pass

rec('Volleyhead')
