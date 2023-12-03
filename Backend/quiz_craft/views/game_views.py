from rest_framework import generics
from rest_framework.response import Response
from rest_framework import status
from ..models.game import Game
from ..permissions import IsTeacher
from ..serializers.game_serializer import GameSerializer


class GameListView(generics.ListCreateAPIView):
    queryset = Game.objects.all()
    serializer_class = GameSerializer
    permission_classes = [IsTeacher]

    def perform_create(self, serializer):
        serializer.save()


class GameDetailView(generics.RetrieveAPIView):
    queryset = Game.objects.all()
    serializer_class = GameSerializer
    permission_classes = [IsTeacher]
