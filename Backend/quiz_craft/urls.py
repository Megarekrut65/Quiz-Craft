from django.urls import path

from .views.game_views import GameListView, GameDetailView, GameUIDCreateView, GameUIDRetrieveDestroyView, \
    GameRetrieveByUIDView
from .views.gpt_task_views import GenerateTaskView
from .views.response_views import TaskResponseCreateView, TaskResponseRetrieveView, TaskResponsesListView, \
    TaskResponseDetailView, QuestionResponseGradeUpdateView, GameResponseCreateView, GameResponseRetrieveView
from .views.task_views import TaskCreateView, TaskListView, TaskDetailView, TaskUIDCreateView, \
    TaskUIDRetrieveDestroyView, TaskRetrieveByUIDView
from .views.user_views import UserProfileListView, UserRegistrationView, UserLoginView, UserLogoutView

urlpatterns = [
    # path('userprofiles/', UserProfileListView.as_view(), name='userprofile-list'),
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
    path('tasks/response/<int:pk>/', TaskResponseDetailView.as_view(), name='task-responses-list'),
    path('games/', GameListView.as_view(), name='game-list'),
    path('games/<int:pk>/', GameDetailView.as_view(), name='game-detail'),
    path('games/generate-uid/', GameUIDCreateView.as_view(), name='game-uid-create'),
    path('games/uid/<int:game_id>/', GameUIDRetrieveDestroyView.as_view(), name='game-uid-retrieve-destroy'),
    path('games/uid/get-game/<str:uid>/', GameRetrieveByUIDView.as_view(), name='uid-get-game'),
    path('question-responses/grade/<int:pk>/', QuestionResponseGradeUpdateView.as_view(), name='question-response-update-grade'),
    path('game-responses/create/', GameResponseCreateView.as_view(), name='game_response_create'),
    path('game-responses/<str:uid>/', GameResponseRetrieveView.as_view(), name='game-response-retrieve'),
    path('generate-task/', GenerateTaskView.as_view(), name='generate-task'),
]
