import numpy as np
from math_actions.BaseMath import BaseMath


class Minus(BaseMath):
    def __init__(self, files, pic=None):
        self.pic = pic
        self.files = files

    def compute(self):
        """
        Method to compute minus
        :return:
        """
        if self.files is None:
            if type(self.pic) in [int, float]:
                return None
            else:
                return self.pic
        elif self.pic is None:
            return self.files
        else:
            files_after_subtraction = []
            for file in self.files:
                outcome_array = np.subtract(file, self.pic)
                files_after_subtraction.append(outcome_array)
            return files_after_subtraction

    def subtract_two_images(self):
        if self.files is None:
            if type(self.pic) in [int, float]:
                return None
            else:
                return self.pic
        elif self.pic is None:
            return self.files
        else:
            return np.subtract(self.files[0], self.files[1])



if __name__ == '__main__':
    a = [
        [1, 1, 1],
        [1, 1, 1]
    ]
    print(Multiplication(a, 2).compute())
    print(Multiplication(None, 0).compute())
    print(Multiplication(None, None).compute())
    print(Multiplication(a, None).compute())