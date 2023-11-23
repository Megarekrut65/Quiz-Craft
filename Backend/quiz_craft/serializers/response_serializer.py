from rest_framework import serializers
from ..models.response import TaskResponse, QuestionResponse


class QuestionResponseSerializer(serializers.ModelSerializer):
    class Meta:
        model = QuestionResponse
        fields = ['question', 'answer']


class TaskResponseSerializer(serializers.ModelSerializer):
    question_responses = QuestionResponseSerializer(many=True)

    class Meta:
        model = TaskResponse
        fields = ['profile', 'task', 'question_responses']
