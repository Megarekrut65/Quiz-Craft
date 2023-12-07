from django.db import IntegrityError
from django.shortcuts import get_object_or_404
from django.utils import timezone
from rest_framework import generics, status
from rest_framework.permissions import IsAuthenticated
from rest_framework.response import Response

from ..models.game import GameUID
from ..models.response import TaskResponse, QuestionResponse
from ..models.task import Task, Question, Answer, TaskUID
from ..permissions import IsTeacher
from ..serializers.response_serializer import TaskResponseSerializer, TaskResponseListSerializer, \
    TaskResponseDetailSerializer, QuestionResponseSerializer


class TaskResponseCreateView(generics.CreateAPIView):
    serializer_class = TaskResponseSerializer
    permission_classes = [IsAuthenticated]

    def post(self, request, *args, **kwargs):
        uid = request.data.get('uid')
        question_responses_data = request.data.get('question_responses', [])

        try:
            task_uid = TaskUID.objects.get(uid=uid)
            task = task_uid.task
        except TaskUID.DoesNotExist:
            return Response({"detail": "Task UID not found."}, status=status.HTTP_404_NOT_FOUND)

        task_response = TaskResponse(profile=request.user.userprofile, task=task)

        question_responses = []
        for response_data in question_responses_data:
            question_id = response_data.get('question_id')
            answer_id = response_data.get('answer_id')
            text_answer = response_data.get('text_answer', '')

            try:
                question = Question.objects.get(id=question_id, task=task)
            except Question.DoesNotExist:
                return Response({"detail": f"Question with id {question_id} not found for the given task."},
                                status=status.HTTP_404_NOT_FOUND)

            if question.type == 'TEXT':
                question_response = QuestionResponse(
                    task_response=task_response,
                    question=question,
                    text_answer=text_answer
                )
            else:
                try:
                    answer = Answer.objects.get(id=answer_id, question=question)
                except Answer.DoesNotExist:
                    return Response({"detail": f"Answer with id {answer_id} not found for the given question."},
                                    status=status.HTTP_404_NOT_FOUND)

                question_response = QuestionResponse(
                    task_response=task_response,
                    question=question,
                    answer=answer
                )

            question_responses.append(question_response)

        try:
            task_response.save(force_insert=True)
        except IntegrityError:
            return Response({"detail": "You have already responded to this task."},
                            status=status.HTTP_400_BAD_REQUEST)

        for question_response in question_responses:
            question_response.save(force_insert=True)

        task_response_serializer = TaskResponseSerializer(task_response)

        response_data = {
            'task_response': task_response_serializer.data
        }

        return Response(response_data, status=status.HTTP_201_CREATED)


class TaskResponseRetrieveView(generics.RetrieveAPIView):
    serializer_class = TaskResponseSerializer
    permission_classes = [IsAuthenticated]

    def get_queryset(self):
        uid = self.kwargs.get('uid')
        try:
            task_uid = TaskUID.objects.get(uid=uid)
        except TaskUID.DoesNotExist:
            return TaskResponse.objects.none()
        return TaskResponse.objects.filter(task=task_uid.task, profile=self.request.user.userprofile)

    def retrieve(self, request, *args, **kwargs):
        queryset = self.get_queryset()
        if not queryset.exists():
            return Response({"detail": "No response found for the given task and user, or owner closed the task."},
                            status=status.HTTP_404_NOT_FOUND)

        instance = queryset.first()
        serializer = self.get_serializer(instance)
        return Response(serializer.data, status=status.HTTP_200_OK)


class TaskResponsesListView(generics.ListAPIView):
    serializer_class = TaskResponseListSerializer
    permission_classes = [IsTeacher]

    def get_queryset(self):
        task_id = self.kwargs.get('task_id')
        task = get_object_or_404(Task, id=task_id, created_by=self.request.user.userprofile)
        return TaskResponse.objects.filter(task=task)

    def list(self, request, *args, **kwargs):
        queryset = self.get_queryset()
        serializer = self.get_serializer(queryset, many=True)
        return Response(serializer.data, status=status.HTTP_200_OK)


class TaskResponseDetailView(generics.RetrieveAPIView):
    serializer_class = TaskResponseDetailSerializer
    permission_classes = [IsTeacher]

    def get_queryset(self):
        response_id = self.kwargs.get('pk')
        user_profile = self.request.user.userprofile

        return TaskResponse.objects.filter(id=response_id, task__created_by=user_profile)

    def retrieve(self, request, *args, **kwargs):
        instance = self.get_object()
        serializer = self.get_serializer(instance)
        return Response(serializer.data, status=status.HTTP_200_OK)


class QuestionResponseGradeUpdateView(generics.UpdateAPIView):
    serializer_class = QuestionResponseSerializer
    permission_classes = [IsTeacher]
    queryset = QuestionResponse.objects.all()

    def update(self, request, *args, **kwargs):
        instance = self.get_object()
        grade = request.data.get('grade')

        if request.user.userprofile != instance.task_response.task.created_by:
            return Response({"detail": "You don't have permission to update the grade for this question response."},
                            status=status.HTTP_403_FORBIDDEN)

        instance.grade = grade
        instance.save()

        serializer = self.get_serializer(instance)
        return Response(serializer.data, status=status.HTTP_200_OK)


class GameResponseCreateView(generics.CreateAPIView):
    serializer_class = TaskResponseSerializer
    permission_classes = [IsAuthenticated]

    def post(self, request, *args, **kwargs):
        uid = request.data.get('uid')
        question_id = request.data.get('question_id')
        answer_id = request.data.get('answer_id', None)

        try:
            game_uid = GameUID.objects.get(uid=uid)
            task = game_uid.game.task
        except GameUID.DoesNotExist:
            return Response({"detail": "Game UID not found."}, status=status.HTTP_404_NOT_FOUND)

        try:
            question = Question.objects.get(id=question_id, task=task)
        except Question.DoesNotExist:
            return Response({"detail": f"Question with id {question_id} not found for the given task."},
                            status=status.HTTP_404_NOT_FOUND)

        if answer_id is None:
            return Response({"detail": "Answer ID is required for this question type."},
                            status=status.HTTP_400_BAD_REQUEST)

        try:
            answer = Answer.objects.get(id=answer_id, question=question)
        except Answer.DoesNotExist:
            return Response({"detail": f"Answer with id {answer_id} not found for the given question."},
                            status=status.HTTP_404_NOT_FOUND)

        task_response, created = TaskResponse.objects.get_or_create(profile=request.user.userprofile, task=task)

        if not created:
            task_response.created_at = timezone.now()
            task_response.save()

        question_response = QuestionResponse.objects.filter(task_response=task_response, question=question)

        if question_response:
            return Response({"detail": "You have already responded to this question."},
                            status=status.HTTP_400_BAD_REQUEST)

        question_response = QuestionResponse(
            task_response=task_response,
            question=question,
            answer=answer
        )

        question_response.save(force_insert=True)

        response_data = {
            'answer_id': answer.id,
            'correct': answer.correct
        }

        return Response(response_data, status=status.HTTP_201_CREATED)


class GameResponseRetrieveView(generics.RetrieveAPIView):
    serializer_class = TaskResponseSerializer
    permission_classes = [IsAuthenticated]

    def get_queryset(self):
        uid = self.kwargs.get('uid')
        try:
            game_uid = GameUID.objects.get(uid=uid)
        except GameUID.DoesNotExist:
            return TaskResponse.objects.none()

        task = game_uid.game.task
        return TaskResponse.objects.filter(task=task, profile=self.request.user.userprofile)

    def retrieve(self, request, *args, **kwargs):
        queryset = self.get_queryset()
        if not queryset.exists():
            return Response({"detail": "No response found for the given game and user, or owner closed the game."},
                            status=status.HTTP_404_NOT_FOUND)

        instance = queryset.first()
        serializer = self.get_serializer(instance)
        return Response(serializer.data, status=status.HTTP_200_OK)
