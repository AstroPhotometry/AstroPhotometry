import numpy as np
from math_actions.BaseMath import BaseMath


class Median(BaseMath):

    def __init__(self, files):
        self.files = files

    def compute(self):
        """
        Method to compute average
        :return:
        """
        if self.files is None:
            return None
        out = np.array(self.files)
        return np.median(out, axis=0)


if __name__ == '__main__':
    a = [[
        [1, 1, 1],
        [1, 1, 1]
    ],
        [
            [3, 3, 3],
            [3, 3, 3]
        ],
    ]
    print(Median(a).compute())

