from django.contrib import admin

from .models.response import TaskResponse, QuestionResponse
from .models.user import UserProfile
from .models.task import Task, Question, Answer, TaskUID

# Register your models here.


admin.site.register(UserProfile)
admin.site.register(Task)
admin.site.register(Question)
admin.site.register(Answer)
admin.site.register(TaskResponse)
admin.site.register(QuestionResponse)
admin.site.register(TaskUID)
