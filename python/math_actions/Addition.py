import numpy as np
from math_actions.BaseMath import BaseMath


class Addition(BaseMath):

    def __init__(self, image1, image2):
        self.image1 = image1
        self.image2 = image2

    def compute(self):
        """
        Method to compute addition
        :return:
        """
        if self.image1 is None:
            if type(self.image2) in [int, float]:
                return None
            else:
                return self.image2
        elif self.image2 is None:
            return self.image1
        elif type(self.image2) in [int, float]:
            return np.add(self.image1, self.image2)
        else:
            out = np.array([self.image1, self.image2])
            return np.sum(out, axis=0)


if __name__ == '__main__':
    a = [
        [1, 1, 1],
        [1, 1, 1]
    ]
    print(Addition(a, 2).compute())
    print(Addition(None, 0).compute())
    print(Addition(None, None).compute())
    print(Addition(a, None).compute())
    print(Addition(None, a).compute())
