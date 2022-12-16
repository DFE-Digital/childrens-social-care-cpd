import threading
import time
import requests

# Number of threads to create
thread_count = 25

# URL of the website to stress test
url = "https://test.develop-child-family-social-work-career.education.gov.uk/CPD/LandingPage"

def make_request():
    # Send a GET request to the website
    response = requests.get(url)
    
    # Print the status code of the response
    if response.status_code == 200:
        print("*", end="")
    else:
        print(".", end="")

# Create a list to store the threads
threads = []

# Run the stress test in a loop until the user presses 'q' or 'Q'
while True:
    # Create the specified number of threads
    for i in range(thread_count):
        thread = threading.Thread(target=make_request)
        thread.start()
        threads.append(thread)

    # Wait for all threads to complete
    for thread in threads:
        thread.join()

    print(" Done!")
