from django.contrib.auth import authenticate, login, logout
from django.contrib.auth.models import User
from rest_framework import generics, status
from rest_framework.authtoken.models import Token
from rest_framework.permissions import IsAuthenticated
from rest_framework.views import APIView
from rest_framework.response import Response

from ..models.user import UserProfile
from ..serializers.user_serializer import UserProfileSerializer, UserRegistrationSerializer, UserSerializer, \
    UserLoginSerializer


class UserProfileListView(generics.ListAPIView):
    queryset = UserProfile.objects.all()
    serializer_class = UserProfileSerializer


class UserRegistrationView(APIView):
    def post(self, request, *args, **kwargs):
        if request.user.is_authenticated:
            return Response({'message': 'Registration not allowed for authenticated users.'},
                            status=status.HTTP_400_BAD_REQUEST)

        serializer = UserRegistrationSerializer(data=request.data)
        if serializer.is_valid():
            user = serializer.save()
            user_serializer = UserSerializer(user)
            return Response(user_serializer.data, status=status.HTTP_201_CREATED)
        return Response(serializer.errors, status=status.HTTP_400_BAD_REQUEST)


class UserLoginView(APIView):
    def post(self, request, *args, **kwargs):
        serializer = UserLoginSerializer(data=request.data)
        if serializer.is_valid():
            username = serializer.validated_data['username']
            password = serializer.validated_data['password']

            user = authenticate(request, username=username, password=password)

            if user:
                login(request, user)
                token, created = Token.objects.get_or_create(user=user)

                return Response({'token': token.key, 'message': 'Login successful.'}, status=status.HTTP_200_OK)

            return Response({'message': 'Invalid credentials.'}, status=status.HTTP_401_UNAUTHORIZED)

        return Response(serializer.errors, status=status.HTTP_400_BAD_REQUEST)


class UserLogoutView(APIView):
    permission_classes = [IsAuthenticated]

    def post(self, request, *args, **kwargs):
        logout(request)
        return Response({'message': 'Logout successful.'}, status=status.HTTP_200_OK)
