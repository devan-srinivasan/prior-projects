This program is a simulation of the genetic algorithm, otherwise known as the natural selection
algorithm. It shows how a population of entities can learn to reach a goal by following a set algorithm.
The algorithm works as follows:
1) create a population of agents (dots in this case) that move randomly
2) calculate the fitness score of each agent based on its proximity to a goal
3) the higher the fitness of a dot the higher chance it has to make children for the next generation of dots
4) create a new generation of dots, children of the previous. Mutate them slightly to allow change.
And repeat the process

As the process goes on the dots will be smarter as the dots that scored higher fitness values
(meaning they were closer to the goal) made more children for the new generation. We mutate the children slightly
so they don't make their parent's mistakes.

Inspired by CodeBullet on YouTube.