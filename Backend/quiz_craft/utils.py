from django.utils.crypto import get_random_string
from .models.task import TaskUID


def generate_unique_uid():
    while True:
        uid = get_random_string(length=32)
        if not TaskUID.objects.filter(uid=uid).exists():
            break
    return uid
