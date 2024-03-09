# Maze-Agent
Using Unity's MLAgents library to implement a reinforcement agent that traverses the maze to reach the target goal.

## Action Space
The agent is able to move in the X, Z, or a combination of both, directions. This allows the agent to roam freely.

## Observation Space
The environment keeps track of the agents position and the targets position.

## Reward System
The agent is penalized for each step (preventing the agent from remaining still) and for hitting the wall. The agent is rewarded when their new position is closer to the target than its previous position. Once the agent reaches a goal, the agent is rewarded significantly for doing so, incentivizing them to continue reaching that target.
