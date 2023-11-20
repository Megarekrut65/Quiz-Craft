from django.contrib import admin
from .models.user import UserProfile
from .models.task import Task, Question, Answer
# Register your models here.


admin.site.register(UserProfile)
admin.site.register(Task)
admin.site.register(Question)
admin.site.register(Answer)
