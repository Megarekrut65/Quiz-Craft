from rest_framework import generics, status
from rest_framework.permissions import IsAuthenticated
from rest_framework.response import Response

from ..models.task import Task, TaskUID
from ..permissions import IsTeacher
from ..serializers.task_serializer import TaskCreateSerializer, TaskListSerializer, TaskUIDSerializer, \
    TaskByUIDSerializer
from ..utils.utils import generate_unique_task_uid


class TaskCreateView(generics.CreateAPIView):
    serializer_class = TaskCreateSerializer
    permission_classes = [IsTeacher]

    def perform_create(self, serializer):
        serializer.save(created_by=self.request.user.userprofile)

    def create(self, request, *args, **kwargs):
        serializer = self.get_serializer(data=request.data, context={'request': request})
        serializer.is_valid(raise_exception=True)
        self.perform_create(serializer)
        headers = self.get_success_headers(serializer.data)
        return Response(serializer.data, status=status.HTTP_201_CREATED, headers=headers)


class TaskListView(generics.ListAPIView):
    serializer_class = TaskListSerializer
    permission_classes = [IsTeacher]

    def get_queryset(self):
        user_profile = self.request.user.userprofile
        return Task.objects.filter(created_by=user_profile)


class TaskDetailView(generics.RetrieveAPIView):
    serializer_class = TaskListSerializer
    permission_classes = [IsTeacher]

    def get_queryset(self):
        user_profile = self.request.user.userprofile
        return Task.objects.filter(created_by=user_profile)


class TaskUIDCreateView(generics.CreateAPIView):
    serializer_class = TaskUIDSerializer
    permission_classes = [IsTeacher]

    def create(self, request, *args, **kwargs):
        task_id = request.data['task_id']
        try:
            task = Task.objects.get(id=task_id, created_by=request.user.userprofile)
        except Task.DoesNotExist:
            return Response({"detail": "Task not found or you are not the creator."}, status=status.HTTP_404_NOT_FOUND)

        if TaskUID.objects.filter(task_id=task_id).exists():
            return Response({'detail': 'UID already exists for task ID {}'.format(task_id)},
                            status=status.HTTP_400_BAD_REQUEST)

        uid = generate_unique_task_uid()
        task_uid = TaskUID.objects.create(uid=uid, task=task)

        serializer = self.get_serializer(task_uid)
        headers = self.get_success_headers(serializer.data)
        return Response(serializer.data, status=status.HTTP_201_CREATED, headers=headers)


class TaskUIDRetrieveDestroyView(generics.RetrieveDestroyAPIView):
    serializer_class = TaskUIDSerializer
    permission_classes = [IsTeacher]

    def get_queryset(self):
        task_id = self.kwargs.get('task_id')
        return TaskUID.objects.filter(task_id=task_id, task__created_by=self.request.user.userprofile)

    def retrieve(self, request, *args, **kwargs):
        queryset = self.get_queryset()
        if not queryset.exists():
            return Response({"detail": "TaskUID not found for the specified Task ID or you are not the creator."},
                            status=status.HTTP_404_NOT_FOUND)

        instance = queryset.first()
        serializer = self.get_serializer(instance)
        return Response(serializer.data, status=status.HTTP_200_OK)

    def destroy(self, request, *args, **kwargs):
        queryset = self.get_queryset()
        if not queryset.exists():
            return Response({"detail": "TaskUID not found for the specified Task ID or you are not the creator."},
                            status=status.HTTP_404_NOT_FOUND)

        instance = queryset.first()
        instance.delete()
        return Response({"message": "TaskUID deleted successfully."}, status=status.HTTP_204_NO_CONTENT)


class TaskRetrieveByUIDView(generics.RetrieveAPIView):
    serializer_class = TaskByUIDSerializer
    permission_classes = [IsAuthenticated]

    def get_queryset(self):
        uid = self.kwargs.get('uid')
        return Task.objects.filter(task_uid__uid=uid)

    def retrieve(self, request, *args, **kwargs):
        queryset = self.get_queryset()
        if not queryset.exists():
            return Response({"detail": "Task not found for the specified UID."},
                            status=status.HTTP_404_NOT_FOUND)

        instance = queryset.first()
        serializer = self.get_serializer(instance)
        return Response(serializer.data, status=status.HTTP_200_OK)
