# Generated by Django 4.2.6 on 2023-12-04 15:18

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('quiz_craft', '0012_gameuid'),
    ]

    operations = [
        migrations.AddField(
            model_name='questionresponse',
            name='grade',
            field=models.IntegerField(blank=True, null=True),
        ),
    ]
