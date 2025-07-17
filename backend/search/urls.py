from django.urls import path
from .views import MasterListItemListCreate

urlpatterns = [
    path('items/', MasterListItemListCreate.as_view(), name='masterlistitem-list'),
]
