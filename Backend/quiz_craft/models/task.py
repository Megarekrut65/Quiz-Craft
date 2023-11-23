from django.db import models
from ..models.user import UserProfile


class Answer(models.Model):
    option = models.CharField(max_length=100)
    correct = models.BooleanField()
    question = models.ForeignKey('Question', on_delete=models.CASCADE, related_name='answers', default=1)


class Question(models.Model):
    QUESTION_TYPES = [
        ('SINGLE', 'Single Choice'),
        ('MULTI', 'Multiple Choice'),
        ('TEXT', 'Text Answer'),
    ]

    type = models.CharField(max_length=10, choices=QUESTION_TYPES)
    description = models.CharField(max_length=200)
    max_grade = models.IntegerField()
    task = models.ForeignKey('Task', on_delete=models.CASCADE, related_name='questions', default=1)


class Task(models.Model):
    title = models.CharField(max_length=500)
    description = models.TextField(max_length=1000)
    created_by = models.ForeignKey(UserProfile, on_delete=models.CASCADE, related_name='created_tasks', default=1)


class TaskUID(models.Model):
    uid = models.CharField(max_length=32, unique=True)
    task = models.OneToOneField(Task, on_delete=models.CASCADE, related_name='task_uid')
