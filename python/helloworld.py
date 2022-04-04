from asyncore import write
import sys


if __name__ == "__main__":
    argument = sys.argv[0] + "  " + sys.argv[1]

    file = open("test.txt","+w")
    file.write(argument)
    file.close()
