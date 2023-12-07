from rest_framework import generics
from rest_framework.permissions import IsAuthenticated
from rest_framework.response import Response
from rest_framework import status
from ..models.game import Game, GameUID
from ..permissions import IsTeacher
from ..serializers.game_serializer import GameSerializer, GameUIDSerializer, GameByUIDSerializer
from ..utils.utils import generate_unique_game_uid


class GameListView(generics.ListCreateAPIView):
    serializer_class = GameSerializer
    permission_classes = [IsTeacher]

    def get_queryset(self):
        return Game.objects.filter(task__created_by=self.request.user.userprofile)

    def perform_create(self, serializer):
        serializer.save()


class GameDetailView(generics.RetrieveAPIView):
    serializer_class = GameSerializer
    permission_classes = [IsTeacher]

    def get_queryset(self):
        return Game.objects.filter(task__created_by=self.request.user.userprofile)


class GameUIDCreateView(generics.CreateAPIView):
    serializer_class = GameUIDSerializer
    permission_classes = [IsTeacher]

    def create(self, request, *args, **kwargs):
        game_id = request.data['game_id']
        try:
            game = Game.objects.get(id=game_id)
            if game.task.created_by != request.user.userprofile:
                return Response({"detail": "You are not the creator of this game."},
                                status=status.HTTP_403_FORBIDDEN)
        except Game.DoesNotExist:
            return Response({"detail": "Game not found."}, status=status.HTTP_404_NOT_FOUND)

        if GameUID.objects.filter(game_id=game_id).exists():
            return Response({'detail': 'UID already exists for game ID {}'.format(game_id)},
                            status=status.HTTP_400_BAD_REQUEST)

        uid = generate_unique_game_uid()
        game_uid = GameUID.objects.create(uid=uid, game=game)

        serializer = self.get_serializer(game_uid)
        headers = self.get_success_headers(serializer.data)
        return Response(serializer.data, status=status.HTTP_201_CREATED, headers=headers)


class GameUIDRetrieveDestroyView(generics.RetrieveDestroyAPIView):
    serializer_class = GameUIDSerializer
    permission_classes = [IsTeacher]

    def get_queryset(self):
        game_id = self.kwargs.get('game_id')
        return GameUID.objects.filter(game_id=game_id, game__task__created_by=self.request.user.userprofile)

    def retrieve(self, request, *args, **kwargs):
        queryset = self.get_queryset()
        if not queryset.exists():
            return Response({"detail": "GameUID not found for the specified Game ID or you are not the creator."},
                            status=status.HTTP_404_NOT_FOUND)

        instance = queryset.first()
        serializer = self.get_serializer(instance)
        return Response(serializer.data, status=status.HTTP_200_OK)

    def destroy(self, request, *args, **kwargs):
        queryset = self.get_queryset()
        if not queryset.exists():
            return Response({"detail": "GameUID not found for the specified Game ID or you are not the creator."},
                            status=status.HTTP_404_NOT_FOUND)

        instance = queryset.first()
        instance.delete()
        return Response({"message": "GameUID deleted successfully."}, status=status.HTTP_204_NO_CONTENT)


class GameRetrieveByUIDView(generics.RetrieveAPIView):
    serializer_class = GameByUIDSerializer
    permission_classes = [IsAuthenticated]

    def get_queryset(self):
        uid = self.kwargs.get('uid')
        return Game.objects.filter(game_uid__uid=uid)

    def retrieve(self, request, *args, **kwargs):
        queryset = self.get_queryset()
        if not queryset.exists():
            return Response({"detail": "Game not found for the specified UID."},
                            status=status.HTTP_404_NOT_FOUND)

        instance = queryset.first()
        serializer = self.get_serializer(instance)
        return Response(serializer.data, status=status.HTTP_200_OK)
