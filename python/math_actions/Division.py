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
    def __init__(self, image, scalar):
        self.scalar = scalar
        self.image = image

    def compute(self):
        """
        Method to compute division - matrix / scalar
        :return:
        """
        if self.image is None:
            return None
        elif self.scalar is None:
            return self.image
        else:
            outcome = div_matrix(self.image, self.scalar)
            return outcome
