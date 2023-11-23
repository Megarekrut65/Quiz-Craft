from django.urls import path

from .views.response_views import TaskResponseCreateView, TaskResponseRetrieveView, TaskResponsesListView
from .views.task_views import TaskCreateView, TaskListView, TaskDetailView, TaskUIDCreateView, \
    TaskUIDRetrieveDestroyView, TaskRetrieveByUIDView
from .views.user_views import UserProfileListView, UserRegistrationView, UserLoginView, UserLogoutView

urlpatterns = [
    path('userprofiles/', UserProfileListView.as_view(), name='userprofile-list'),
    path('register/', UserRegistrationView.as_view(), name='user-registration'),
    path('login/', UserLoginView.as_view(), name='user-login'),
    path('logout/', UserLogoutView.as_view(), name='user-logout'),
    path('tasks/', TaskListView.as_view(), name='task-list'),
    path('tasks/create/', TaskCreateView.as_view(), name='task-create'),
    path('tasks/<int:pk>/', TaskDetailView.as_view(), name='task-detail'),
    path('tasks/generate-uid/', TaskUIDCreateView.as_view(), name='task-generate-uid'),
    path('tasks/uid/<int:task_id>/', TaskUIDRetrieveDestroyView.as_view(), name='task-get-delete-uid'),
    path('tasks/uid/get-task/<str:uid>/', TaskRetrieveByUIDView.as_view(), name='uid-get-task'),
    path('task-responses/create/', TaskResponseCreateView.as_view(), name='task_response_create'),
    path('task-responses/<str:uid>/', TaskResponseRetrieveView.as_view(), name='task-response-retrieve'),
    path('tasks/responses/<int:task_id>/', TaskResponsesListView.as_view(), name='task-responses-list'),
]
