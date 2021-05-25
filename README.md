## About The Project
Large files transfers are under tremendous pressure. How to transfer trivial files(MegaBytes) with a large total size(Terabytes)? 

One method is direct transfer. But in this way, you may pray for a smooth transmission. Otherwise, once error occurs, it is difficult to find out which file encountered the error. So must transfer all again!

Another way is archive it first to become a large file. Then transfer this large file. This is a good way, but you may spend a lot of time on archive files.

So how if we archive this from the other way? For example, archive each directory in the root folder. In this method, if we use other archive software such as `7z`, `winRAR`, there is no pragrammable parallel configuration to speed up the archiving process (as far as I know).

This project was developed to take advantage of parallelism to speed up the archiving process. Archiving multiples files or directories at once!

In addition to archiving, the project also has hash functions, such as `md5`, `sha256`. Hash is used for check file integrity. Also, it is implemented on parallelism, which can hash multiple files at once.

## Usage
### Archive
- Archive each directory or file in root folder `E:\data` (Deepth only 1) and store these archived file to other folder `E:\data1`. With maximum parallelism 5
    ```ps
    .\Utils.exe compress-seperate -s "E:\data" -d "E:\data1" --timestamp --core-num 5
    ```
    - `-s` is root directory where all waiting archive directories or files are located and `-d` is archive destination.
    - When `--timestamp` is set, a log with a timestamp is recorded.
- Archive file with password "`thisispassword`"
    ```ps
    .\Utils.exe compress-seperate -s "E:\data" -d "E:\data1" --timestamp --core-num 5 -p thisispassword
    ```
    - `-p` is the archive password.
### Hash
- Calculate `md5` of each archived file in `E:\data1`
    ```ps
    .\Utils.exe hash -d -h md5 -p "E:\data1" --core-num 8
    ```
    - `-d` is means that the value of path `-p` value is a directory, otherwise it is a the file.
    - `-h` choose a hash algorithm.
    - `--core-num` is maximum parallelism.