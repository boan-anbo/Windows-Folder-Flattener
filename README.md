# Windows-Folder-Flattener
* Flattens folders on Windows.

# What it does
* If you have something like below: a folder (`root_directory`) with multiple sub-folders (`a`, `b`, `c`), each of which has files (e.g. `a_1.txt`) and further sub-folders (e.g. `c_2`), you can use it to move all those files to the root directory, hence flattening the base folder. 
```
C:\root_directory
        +---a
        |   \---a_1
        |           a_1.txt
        |
        +---b
        |       b_1.txt
        |
        \---c
            \---c_1
                \---c_2
                        c_2.txt
```
# Usage
* Run `flattener c:\root_directory`. The above folder will be flattened into (notice now the sub-folders like `a`, `b`, `c`, `a_1`, `c_1`, `c_2` are emptied):
```
C:\root_directory
        |   a_1.txt
        |   b_1.txt
        |   c_2.txt
        |
        +---a
        |   \---a_1
        +---b
        \---c
            \---c_1
                \---c_2
```
* If you don't want to keep those empitied sub-folders, you could have ran `flattener c:\root_directory -d` instead. You will get:
```
C:\root_directory
      |   a_1.txt
      |   b_1.txt
      |   c_2.txt
```
# 2nd Usage
* You can also do the same with the sub-folders as the "sub-root folder", and move the files under them not to the one root folder, but to their respective "sub-root folder". 
* To do this, you can run `flattener c:\root_directory -s`. That will get you:
```
C:\root_directory
+---a
|   |   a_1.txt
|   |
|   \---a_1
+---b
|       b_1.txt
|
\---c
    \---c_1
        \---c_2
```
* Again, you can clean it with `flattener c:\root_directory -s -d` so that you have:
```
C:\root_directory
      +---a
      |       a_1.txt
      |
      +---b
      |       b_1.txt
      |
      \---c
              c_2.txt
```
# Options
* `-c`: use the current folder.
* `-d`: delete empty sub-folders after flattening.
* `-s`: use immediate sub-folders as "sub root folders".

# Change Log

2020-01-05: v 0.1
