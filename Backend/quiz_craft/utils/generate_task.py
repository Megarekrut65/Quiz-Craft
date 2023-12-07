import json
import re
import openai
from quiz_craft_backend.settings import GPT_API

JSON_FORMAT = """
{
    "title": "Sample Task",
    "description": "This is a sample task description.",
    "questions": [
        {
            "type": "SINGLE",
            "description": "Sample description 1",
            "max_grade": 1,
            "answers": [
                {
                    "option": "sample answer 1",
                    "correct": false
                },
                {
                    "option": "sample answer 2",
                    "correct": true
                },
                {
                    "option": "sample answer 3",
                    "correct": false
                }
            ]
        },
        {
            "type": "MULTI",
            "description": "Sample description 2",
            "max_grade": 1,
            "answers": [
                {
                    "option": "sample answer 4",
                    "correct": false
                },
                {
                    "option": "sample answer 5",
                    "correct": true
                },
                {
                    "option": "sample answer 6",
                    "correct": false
                }
            ]
        },
        {
            "type": "TEXT",
            "description": "Sample description 3",
            "max_grade": 1
        }
    ]
}
"""


def extract_json(data):
    json_match = re.search(r'```json\n(.+?)```', data, re.DOTALL)

    if json_match:
        json_content = json_match.group(1)

        try:
            parsed_json = json.loads(json_content)
            return parsed_json
        except json.JSONDecodeError as e:
            print(f"Error decoding JSON: {e}")

    return None


def generate_task(theme, questions, answers):
    request = f"Create test with {questions} questions with {answers} answers each on the topic '{theme}' " \
              f"in next JSON format:\n```json\n{json.dumps(json.loads(JSON_FORMAT))}\n```"

    client = openai.Client(api_key=GPT_API)

    response = client.chat.completions.create(
        model="gpt-3.5-turbo",
        messages=[
            {"role": "system", "content": "You are a helpful assistant."},
            {"role": "user", "content": request},
        ]
    )

    for choice in response.choices:
        task = extract_json(choice.message.content)
        if task:
            return task

    return None
