from django.views import View
from rest_framework import status
from rest_framework.response import Response
from rest_framework.views import APIView

from ..permissions import IsTeacher
from ..utils.generate_task import generate_task


class GenerateTaskView(APIView):
    permission_classes = [IsTeacher]

    def post(self, request, *args, **kwargs):
        theme = request.data['theme']
        questions = request.data['questions']
        answers = request.data['answers']

        generated_task = generate_task(theme, questions, answers)

        if generated_task is not None:
            return Response(generated_task, status=status.HTTP_200_OK)

        return Response({'detail': 'Error while generating task'}, status=status.HTTP_500_INTERNAL_SERVER_ERROR)
