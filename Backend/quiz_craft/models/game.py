from django.db import models
from .task import Task


class Game(models.Model):
    GAME_TYPES = [
        ('SINGLEANSWER', 'Single Answer'),
    ]

    WEAPONS = [
        ('SWORD', 'Sword'),
        ('HAMMER', 'Hammer'),
        ('SCYTHE', 'Scythe'),
    ]

    type = models.CharField(max_length=20, choices=GAME_TYPES)
    min_win_grade = models.IntegerField()
    player_weapon = models.CharField(max_length=20, choices=WEAPONS)
    enemy_weapon = models.CharField(max_length=20, choices=WEAPONS)
    task = models.ForeignKey(Task, on_delete=models.CASCADE)
