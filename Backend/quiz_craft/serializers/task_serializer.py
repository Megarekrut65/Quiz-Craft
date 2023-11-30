from rest_framework import serializers
from ..models.task import Task, Answer, Question, TaskUID


class AnswerSerializer(serializers.ModelSerializer):
    class Meta:
        model = Answer
        fields = ['id', 'option', 'correct']


class QuestionSerializer(serializers.ModelSerializer):
    answers = AnswerSerializer(many=True)

    class Meta:
        model = Question
        fields = ['type', 'description', 'max_grade', 'answers']

    def create(self, validated_data):
        answers_data = validated_data.pop('answers')
        question = Question.objects.create(**validated_data)

        for answer_data in answers_data:
            Answer.objects.create(question=question, **answer_data)

        return question


class TaskCreateSerializer(serializers.ModelSerializer):
    questions = QuestionSerializer(many=True)

    class Meta:
        model = Task
        fields = ['id', 'title', 'description', 'questions']

    def create(self, validated_data):
        questions_data = validated_data.pop('questions')
        task = Task.objects.create(**validated_data)

        for question_data in questions_data:
            answers_data = question_data.pop('answers')
            question = Question.objects.create(task=task, **question_data)

            for answer_data in answers_data:
                Answer.objects.create(question=question, **answer_data)

        return task


class QuestionListSerializer(serializers.ModelSerializer):
    answers = AnswerSerializer(many=True)

    class Meta:
        model = Question
        fields = ['id', 'type', 'description', 'max_grade', 'answers']


class TaskListSerializer(serializers.ModelSerializer):
    questions = QuestionListSerializer(many=True)

    class Meta:
        model = Task
        fields = ['id', 'title', 'description', 'questions', 'created_by']


class TaskUIDSerializer(serializers.ModelSerializer):
    class Meta:
        model = TaskUID
        fields = ['uid', 'task']


class AnswerByUIDSerializer(serializers.ModelSerializer):
    class Meta:
        model = Answer
        fields = ['id', 'option']


class QuestionByUIDSerializer(serializers.ModelSerializer):
    answers = AnswerByUIDSerializer(many=True)

    class Meta:
        model = Question
        fields = ['id', 'type', 'description', 'max_grade', 'answers']


class TaskByUIDSerializer(serializers.ModelSerializer):
    questions = QuestionByUIDSerializer(many=True)

    class Meta:
        model = Task
        fields = ['title', 'description', 'questions']
