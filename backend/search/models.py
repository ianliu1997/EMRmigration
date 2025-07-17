from django.db import models

class MasterListItem(models.Model):
    code = models.CharField(max_length=50, blank=True)
    description = models.CharField(max_length=200)

    def __str__(self):
        return self.description
