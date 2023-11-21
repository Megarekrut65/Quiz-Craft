from django.urls import path
from .views.task_views import TaskCreateView, TaskListView
from .views.user_views import UserProfileListView, UserRegistrationView, UserLoginView, UserLogoutView

urlpatterns = [
    path('userprofiles/', UserProfileListView.as_view(), name='userprofile-list'),
    path('register/', UserRegistrationView.as_view(), name='user-registration'),
    path('login/', UserLoginView.as_view(), name='user-login'),
    path('logout/', UserLogoutView.as_view(), name='user-logout'),
    path('tasks/', TaskListView.as_view(), name='task-list'),
    path('tasks/create/', TaskCreateView.as_view(), name='task-create'),
]
