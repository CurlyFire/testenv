FROM mcr.microsoft.com/dotnet/core/aspnet:3.1

ADD bin/Release/netcoreapp3.1/publish /opt/testenv

# set Timezone
ENV TZ=America/Toronto

EXPOSE 5000

WORKDIR /opt/testenv
CMD dotnet exec testenv.dll