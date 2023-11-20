from django.contrib.auth.models import User
from rest_framework import serializers
from ..models.user import UserProfile


class UserProfileSerializer(serializers.ModelSerializer):
    class Meta:
        model = UserProfile
        fields = '__all__'


class UserSerializer(serializers.ModelSerializer):
    role = serializers.SerializerMethodField()

    class Meta:
        model = User
        fields = ['id', 'username', 'role']

    @staticmethod
    def get_role(obj):
        try:
            return obj.userprofile.role
        except UserProfile.DoesNotExist:
            return None


class UserRegistrationSerializer(serializers.Serializer):
    username = serializers.CharField(max_length=150)
    password = serializers.CharField(write_only=True)
    role = serializers.ChoiceField(choices=UserProfile.USER_ROLES)

    def create(self, validated_data):
        user = User.objects.create_user(
            username=validated_data['username'],
            password=validated_data['password'],
        )
        user_profile = UserProfile.objects.create(user=user, role=validated_data['role'])
        return user

    def update(self, instance, validated_data):
        pass


class UserLoginSerializer(serializers.Serializer):
    username = serializers.CharField(max_length=150)
    password = serializers.CharField(write_only=True)

    def create(self, validated_data):
        pass

    def update(self, instance, validated_data):
        pass
