from django.contrib.auth.models import User
from rest_framework import serializers
from ..models.user import UserProfile


class UserProfileSerializer(serializers.ModelSerializer):
    class Meta:
        model = UserProfile
        fields = '__all__'


class UserSerializer(serializers.ModelSerializer):
    profile_id = serializers.ReadOnlyField(source='userprofile.id')
    role = serializers.CharField(source='userprofile.role', read_only=True)
    fullname = serializers.CharField(source='userprofile.fullname', read_only=True)

    class Meta:
        model = User
        fields = ['profile_id', 'username', 'fullname', 'role', 'email']


class UserRegistrationSerializer(serializers.Serializer):
    username = serializers.CharField(max_length=150)
    password = serializers.CharField(write_only=True)
    email = serializers.EmailField()
    fullname = serializers.CharField(max_length=255)
    role = serializers.ChoiceField(choices=UserProfile.USER_ROLES)

    def validate_email(self, value):
        if User.objects.filter(email=value).exists():
            raise serializers.ValidationError("A user with this email already exists.")
        return value

    def create(self, validated_data):
        user = User.objects.create_user(
            username=validated_data['username'],
            password=validated_data['password'],
            email=validated_data['email'],
        )
        UserProfile.objects.create(user=user, fullname=validated_data['fullname'], role=validated_data['role'])
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
