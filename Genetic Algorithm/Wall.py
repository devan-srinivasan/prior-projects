"""This is a class for a wall"""
import pygame
from typing import Tuple, List
BLUE = (0, 0, 255)


class Wall:
    """This class represents an obstacle in the simulation that the dots can't touch"""
    height: int
    width: int
    x: int
    y: int
    color: Tuple[int, int, int]
    edges: List[int]

    def __init__(self, height, width, x, y, color=BLUE):
        self.height, self.width = height, width
        self.x, self.y = x, y
        self.color = color
        self.edges = []  # L U R D

    def draw(self, screen) -> None:
        """Draws the wall to the screen"""
        pygame.draw.rect(screen, self.color, [self.x, self.y, self.width, self.height], 0)
