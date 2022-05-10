"""
Do not delete!!
Debugging python window 
"""


def eprint(*args, **kwargs):
    print(*args, file=sys.stderr, **kwargs)


def show_exception_and_exit(exc_type, exc_value, tb):
    import traceback
    traceback.print_exception(exc_type, exc_value, tb)
    eprint("Press key to exit.")
    input()
    sys.exit(-1)


import sys

sys.excepthook = show_exception_and_exit