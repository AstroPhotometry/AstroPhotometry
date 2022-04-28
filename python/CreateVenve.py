import os
import sys
import StopInCaseOfError

if __name__ == "__main__":
    env_name = sys.argv[1]
    path = sys.argv[2].replace('/', '\\') +'\\'
    os.system(f'python3 -m venv "{env_name}" && "{env_name}\\bin\\pip" install -r "{path}requirements.txt"')
