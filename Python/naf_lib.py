# python 3

import sys

# just for test
def do_simple_math(a, b):
    return a + b

# entry point
if __name__ == "__main__":
    # read params
    argsLen = len(sys.argv)
    # need at least two numeric arguments
    if argsLen < 3:
        # raise 
        raise Exception('Pass two numeric arguments')
    print(str(sys.argv))
    arg1 = sys.argv[1]
    arg2 = sys.argv[2]

    if arg1.isnumeric() is False or arg2.isnumeric() is False:
        raise Exception('Numeric, please')
    
    # let execute some function    
    result = do_simple_math(int(arg1), int(arg2))
    print(result)
