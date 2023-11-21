from functools import wraps

from django.contrib.auth.models import AnonymousUser
from django.http import HttpResponseForbidden
from django.contrib.auth.decorators import login_required


def teacher_required(view_func):
    @wraps(view_func)
    def _wrapped_view(request, *args, **kwargs):
        user_profile = getattr(request.user, 'userprofile', None) if not isinstance(request.user, AnonymousUser) else None
        if user_profile and user_profile.role == 'TEACHER':
            return view_func(request, *args, **kwargs)
        else:
            return HttpResponseForbidden("Permission Denied: Only teachers are allowed.")

    return _wrapped_view


def student_required(view_func):
    @wraps(view_func)
    def _wrapped_view(request, *args, **kwargs):
        user_profile = getattr(request.user, 'userprofile', None) if not isinstance(request.user, AnonymousUser) else None
        if user_profile and user_profile.role == 'STUDENT':
            return view_func(request, *args, **kwargs)
        else:
            return HttpResponseForbidden("Permission Denied: Only students are allowed.")

    return _wrapped_view


class TeacherRequiredMixin:
    @classmethod
    def as_view(cls, **initkwargs):
        view = super().as_view(**initkwargs)
        return login_required(teacher_required(view))


class StudentRequiredMixin:
    @classmethod
    def as_view(cls, **initkwargs):
        view = super().as_view(**initkwargs)
        return login_required(student_required(view))
