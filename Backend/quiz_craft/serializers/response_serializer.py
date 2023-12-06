from rest_framework import serializers
from ..models.response import TaskResponse, QuestionResponse


class QuestionResponseSerializer(serializers.ModelSerializer):
    class Meta:
        model = QuestionResponse
        fields = ['id', 'question', 'answer', 'text_answer', 'grade']


class TaskResponseSerializer(serializers.ModelSerializer):
    question_responses = QuestionResponseSerializer(many=True)

    class Meta:
        model = TaskResponse
        fields = ['profile', 'task', 'question_responses', 'created_at']


class TaskResponseListSerializer(serializers.ModelSerializer):
    profile_id = serializers.IntegerField(source='profile.id', read_only=True)
    fullname = serializers.CharField(source='profile.fullname', read_only=True)

    class Meta:
        model = TaskResponse
        fields = ['id', 'profile_id', 'fullname', 'created_at']


class TaskResponseDetailSerializer(serializers.ModelSerializer):
    profile_id = serializers.IntegerField(source='profile.id', read_only=True)
    fullname = serializers.CharField(source='profile.fullname', read_only=True)
    email = serializers.EmailField(source='profile.user.email', read_only=True)
    question_responses = QuestionResponseSerializer(many=True)

    class Meta:
        model = TaskResponse
        fields = ['id', 'profile_id', 'fullname', 'email', 'task', 'question_responses', 'created_at']
