from django.db import models


class Task(models.Model):
    title = models.CharField(max_length=500)
    description = models.TextField(max_length=1000)
    questions = models.ManyToManyField('Question', related_name='tasks')


class Question(models.Model):
    QUESTION_TYPES = [
        ('SINGLE', 'Single Choice'),
        ('MULTI', 'Multiple Choice'),
        ('TEXT', 'Text Answer'),
    ]

    type = models.CharField(max_length=10, choices=QUESTION_TYPES)
    description = models.CharField(max_length=200)
    max_grade = models.IntegerField()
    answers = models.ManyToManyField('Answer', related_name='questions')


class Answer(models.Model):
    option = models.CharField(max_length=100)
    correct = models.BooleanField()
