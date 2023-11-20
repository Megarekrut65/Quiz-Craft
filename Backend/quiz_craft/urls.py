from django.urls import path
from .views.user_views import UserProfileListView, UserRegistrationView, UserLoginView, UserLogoutView

urlpatterns = [
    path('userprofiles/', UserProfileListView.as_view(), name='userprofile-list'),
    path('register/', UserRegistrationView.as_view(), name='user-registration'),
    path('login/', UserLoginView.as_view(), name='user-login'),
    path('logout/', UserLogoutView.as_view(), name='user-logout'),
]
