# Use the official Python image
FROM python:3.9

# Set the working directory in the container
WORKDIR /app
# Copy the FastAPI app files to the container
ADD . /app

RUN apt-get update && \
    apt-get -y install sudo
RUN sudo -i
RUN pip install --upgrade pip
# Install dependencies
RUN pip install --no-cache-dir -r requirements.txt

# Expose the port that the app runs on
EXPOSE 5213

# Command to run the FastAPI app using uvicorn
CMD ["uvicorn", "main:app", "--host", "0.0.0.0", "--port", "5213"]  
