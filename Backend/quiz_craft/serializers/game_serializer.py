from rest_framework import serializers

from .task_serializer import TaskCreateSerializer
from ..models.game import Game, GameUID


class GameSerializer(serializers.ModelSerializer):
    task = TaskCreateSerializer()

    class Meta:
        model = Game
        fields = ['id', 'type', 'min_win_grade', 'player_weapon', 'enemy_weapon', 'task']

    def create(self, validated_data):
        task_data = validated_data.pop('task', {})
        request = self.context.get('request')

        # Pass the request context to the TaskCreateSerializer
        task_serializer = TaskCreateSerializer(data=task_data, context={'request': request})
        task_serializer.is_valid(raise_exception=True)
        task = task_serializer.save()

        game = Game.objects.create(task=task, **validated_data)
        return game


class GameUIDSerializer(serializers.ModelSerializer):
    class Meta:
        model = GameUID
        fields = ['uid', 'game']
        
