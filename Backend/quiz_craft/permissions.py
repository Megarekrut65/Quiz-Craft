from rest_framework.permissions import BasePermission
from .models.user import UserProfile


class IsTeacher(BasePermission):
    def has_permission(self, request, view):
        if request.user.is_authenticated:
            user_profile = UserProfile.objects.get(user=request.user)
            return user_profile.role == 'TEACHER'
        return False
