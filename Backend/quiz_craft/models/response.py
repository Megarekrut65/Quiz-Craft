from django.db import models
from .user import UserProfile
from .task import Task, Question, Answer


class TaskResponse(models.Model):
    profile = models.ForeignKey(UserProfile, on_delete=models.CASCADE)
    task = models.ForeignKey(Task, on_delete=models.CASCADE)

    class Meta:
        unique_together = ['profile', 'task']


class QuestionResponse(models.Model):
    task_response = models.ForeignKey(TaskResponse, on_delete=models.CASCADE, related_name='question_responses')
    question = models.ForeignKey(Question, on_delete=models.CASCADE)
    answer = models.ForeignKey(Answer, on_delete=models.CASCADE)
