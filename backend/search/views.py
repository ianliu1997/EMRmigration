from rest_framework import generics
from .models import MasterListItem
from .serializers import MasterListItemSerializer

class MasterListItemListCreate(generics.ListCreateAPIView):
    queryset = MasterListItem.objects.all()
    serializer_class = MasterListItemSerializer
