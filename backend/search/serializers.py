from rest_framework import serializers
from .models import MasterListItem

class MasterListItemSerializer(serializers.ModelSerializer):
    class Meta:
        model = MasterListItem
        fields = ['id', 'code', 'description']
