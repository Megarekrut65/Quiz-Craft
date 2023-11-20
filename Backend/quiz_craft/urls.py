from django.urls import path
from .views.user_views import UserProfileListCreateView, UserRegistrationView

urlpatterns = [
    path('userprofiles/', UserProfileListCreateView.as_view(), name='userprofile-list-create'),
    path('register/', UserRegistrationView.as_view(), name='user-registration'),
]
