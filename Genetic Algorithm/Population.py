"""Class for a population of dots"""
import pygame
from Dot import Dot
from typing import List, Tuple


class Population:
    """This class represents a population of dots"""
    dots: List[Dot]
    size: int

    def __init__(self, size: int):
        self.dots = []
        self.size = size
        for i in range(self.size):
            new_dot = Dot()
            self.dots.append(new_dot)

    def init_pos(self, sx, sy) -> None:
        """Initialize the position of all dots. Make them all black"""
        for i in range(self.size):
            self.dots[i].x, self.dots[i].y = sx//2, (7*sy)//8
            self.dots[i].color = (0, 0, 0)

    def draw_population(self, screen) -> None:
        """Draw every dot"""
        for dot in self.dots:
            dot.draw(screen)

    def pop_dead(self) -> bool:
        """Check if the whole population died"""
        for i in range(self.size):
            if not self.dots[i].dead:
                return False
        return True

    def pop_fit(self, gx, gy) -> None:
        """Calculate all dot fitnesses"""
        for i in range(self.size):
            self.dots[i].calc_fit(gx, gy)
