import numpy as np

from math_actions.BaseMath import BaseMath


def div_matrix(a, b, fill=np.nan):
    """
    Method to divide matrix - when dividing with 0 the
    outcome is None
    """
    with np.errstate(divide='ignore', invalid='ignore'):
        c = np.true_divide(a, b)
    if np.isscalar(c):
        return c if np.isfinite(c) \
            else fill
    else:
        c[~ np.isfinite(c)] = fill
        return c


class Division(BaseMath):
    def __init__(self, files, pic):
        self.pic = pic
        self.files = files

    def compute(self):
        """
        Method to compute division
        :return:
        """
        files_after_subtraction = []
        for file in self.files:
            outcome = div_matrix(self.pic, file)
            files_after_subtraction.append(outcome)

        return files_after_subtraction
