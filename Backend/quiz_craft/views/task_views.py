from rest_framework import generics, status
from rest_framework.response import Response

from ..models.task import Task
from ..serializers.task_serializer import TaskCreateSerializer, TaskListSerializer
from ..decorators import TeacherRequiredMixin


class TaskCreateView(TeacherRequiredMixin, generics.CreateAPIView):
    serializer_class = TaskCreateSerializer

    def perform_create(self, serializer):
        serializer.save(created_by=self.request.user.userprofile)

    def create(self, request, *args, **kwargs):
        serializer = self.get_serializer(data=request.data)
        serializer.is_valid(raise_exception=True)
        self.perform_create(serializer)
        headers = self.get_success_headers(serializer.data)
        return Response(serializer.data, status=status.HTTP_201_CREATED, headers=headers)


class TaskListView(TeacherRequiredMixin, generics.ListAPIView):
    serializer_class = TaskListSerializer

    def get_queryset(self):
        user_profile = self.request.user.userprofile
        return Task.objects.filter(created_by=user_profile)
